namespace NetControlCommon.Utils
{
    /// <summary>
    /// Wrap
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class W<T>
    {
        public W(T v)
        {
            Value = v;
        }
        public T Value { get; set; }

        public static implicit operator T(W<T> w)
        {
            return w.Value;
        }

        public static implicit operator W<T>(T w)
        {
            return new W<T>(w);
        }
    }
}
