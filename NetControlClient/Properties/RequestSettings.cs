using System.ComponentModel;
using System.Configuration;

namespace NetControlClient.Properties
{
    // Этот класс позволяет обрабатывать определенные события в классе параметров:
    //  Событие SettingChanging возникает перед изменением значения параметра.
    //  Событие PropertyChanged возникает после изменения значения параметра.
    //  Событие SettingsLoaded возникает после загрузки значений параметров.
    //  Событие SettingsSaving возникает перед сохранением значений параметров.
    internal sealed class RequestSettings
    {
        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingChangingEvent.
        }

        private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingsSaving.
        }
    }
}