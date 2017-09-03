using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;
using CommonMVVM.Properties;

namespace CommonMVVM.MVVM
{
    /// <summary>
    ///     Асинхронные команды -  это команды, которые выполняются в отдельных потоках их пула потоков..
    /// </summary>
    public class AsynchronousCommand : Command, INotifyPropertyChanged
    {
        // Благодаря реализации интерфейса INotifyPropertyChanged появляется возможность уведомления при изменении переменной IsExecuting.Так как оба конструктора вызывают метод Initialise, рассмотрим его более подробно:
        /// <summary>
        ///     Команда отмены
        /// </summary>
        private Command _cancelCommand;

        private bool _isCancellationRequested;


        //Все, что тут выполняется, несмотря на обилие кода — просто установка флага IsCancellationRequested в значение true. Инициализация создает объект и позволяет получить к нему доступ.Так же имеется свойство IsCancellationRequested, которое информирует, когда оно меняет свое состояние.

        //    Так же необходимо знать, когда команда в процессе выполнения.Добавим следующий код:
        /// <summary>
        ///     Флаг, отображающий, что команда в процессе выполнения.
        /// </summary>
        private bool _isExecuting;

        private Dispatcher callingDispatcher;

        /// <summary>
        ///     Инициализация нового экземпляра класса без параметров <see cref="AsynchronousCommand" />.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <param name="isExecuting"></param>
        /// <param name="canExecute">
        ///     Если установлено в
        ///     <c>true</c> команда может выполняться.
        /// </param>
        public AsynchronousCommand(Action action, bool isExecuting, bool canExecute = true)
            : base(action, canExecute)
        {
            _isExecuting = isExecuting;
            //  Инициализация команды
            Initialise();
        }

        /// <summary>
        ///     Инициализация нового экземпляра класса с параметрами<see cref="AsynchronousCommand" />.
        /// </summary>
        /// <param name="parameterizedAction">Параметризированное действие.</param>
        /// <param name="isExecuting"></param>
        /// <param name="canExecute"> Если установлено в <c>true</c> [can execute] (может выполняться).</param>
        public AsynchronousCommand(Action<object> parameterizedAction, bool isExecuting, bool canExecute = true)
            : base(parameterizedAction, canExecute)
        {
            _isExecuting = isExecuting;

            //  Инициализация команды
            Initialise();
        }

        /// <summary>
        ///     Получение команды отмены.
        /// </summary>
        public Command CancelCommand => _cancelCommand;


        /// <summary>
        ///     Получить/Установить значение, указывающее, поступила ли команда отмены
        /// </summary>
        /// <value>
        ///     <c>true</c> если есть запрос на отмену; запроса нет -  <c>false</c>.
        /// </value>
        public bool IsCancellationRequested
        {
            get => _isCancellationRequested;
            set
            {
                if (value == _isCancellationRequested) return;
                _isCancellationRequested = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Получение/Установка флага, который показывает, что команда в процессе выполнения..
        /// </summary>
        /// <value>
        ///     <c>true</c> если в процессе выполнения; иначе <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (value == _isExecuting) return;
                _isExecuting = value;
                OnPropertyChanged();
            }
        }


        // Продолжим.Так как у нас есть возможность отмены добавим события Cancelled и PropertyChanged(реализующее интерфейс INotifyPropertyChanged):
        /// <summary>
        ///     The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Инициализация экземпляра
        /// </summary>
        private void Initialise()
        {
            //  Конструктор команды отмены
            _cancelCommand = new Command(
                () =>
                {
                    //  Set the Is Cancellation Requested flag.
                    IsCancellationRequested = true;
                }, true);
        }


        /// <summary>
        ///     Возникает, когда команда отменена.
        /// </summary>
        public event CommandEventHandler Cancelled;

        //   Так же изменился и метод DoExecute.

        /// <summary>
        ///     Выполнение команды.
        /// </summary>
        /// <param name="param">Параметр.</param>
        public override void DoExecute(object param)
        {
            //  Если уже в процессе выполнения, тоне продолжаем.
            if (IsExecuting)
                return;

            //  Вызов выподняющейся команды, что позволяет отменить ее выполнение.
            var args =
                new CancelCommandEventArgs {Parameter = param, Cancel = false};
            InvokeExecuting(args);

            //  Если отмена -  прерываем.
            if (args.Cancel)
                return;

            //  В процессе выполнения.
            IsExecuting = true;


            //          Мы не запускаем команду, если она уже выполняется, однако есть возможность ее отменить и установить флаг выполнения.
            //  Сохранение вызванного диспатчера.
#if !SILVERLIGHT
            callingDispatcher = Dispatcher.CurrentDispatcher;
#else
      callingDispatcher = System.Windows.Application.Current.RootVisual.Dispatcher;
#endif


            //        Мы должны сохранять диспатчер выполняемой команды, так как при выведении данных о процессе выполнения, ссылаемся на соответствующий диспатчер.

            //           Нужно помнить, что процесс сохранения отличается на Silverlight и WPF.
            // Run the action on a new thread from the thread pool
            // (this will therefore work in SL and WP7 as well).
            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    //   Вызов действия.
                    InvokeAction(param);

                    //  Fire the executed event and set the executing state.
                    ReportProgress(
                        () =>
                        {
                            //  Больше не в процессе выполнения.
                            IsExecuting = false;

                            //  если отменили,
                            //  вызвать событие отмены - , если нет – продолжить выполнение.
                            if (IsCancellationRequested)
                                InvokeCancelled(new CommandEventArgs {Parameter = param});
                            else
                                InvokeExecuted(new CommandEventArgs {Parameter = param});

                            //  Юольше не запрашиваем отмену.
                            IsCancellationRequested = false;
                        }
                    );
                }
            );
        }

        private void InvokeCancelled(CommandEventArgs args)
        {
            var cancel = Cancelled;
            //  Вызвать все события
            cancel?.Invoke(this, args);
        }

        //      А теперь про потоки.Используя пул потоков, отправляем функцию InvokeAction в очередь, которая вызовет функцию команды в отдельном потоке.Так же не следует забывать, что ReportProgress в зависимости от диспатчера, и именно тут нужно изменять свойства и вызывать Executed.При вызове диспатчера (после успешного завершения действия), необходимо очистить флаг IsExecuting, а так же вызывать одно из событий: Cancelled или Executed.И так, осталось рассмотреть только ReportProgress:
        /// <summary>
        ///     Reports progress on the thread which invoked the command.
        /// </summary>
        /// <param name="action">The action.</param>
        public void ReportProgress(Action action)
        {
            if (IsExecuting)
                if (callingDispatcher.CheckAccess())
                    action();
                else
                    callingDispatcher.BeginInvoke((Action) (() => { action(); }));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}