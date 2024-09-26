namespace NauRa.ClockApp.Rest
{
    public readonly struct RestResult<T>
    {
        public bool IsSuccess { get; }
        private readonly T _result;

        private RestResult(bool isSuccess, T result)
        {
            IsSuccess = isSuccess;
            _result = result;
        }

        public static RestResult<T> FromSuccess(T model)
        {
            return new RestResult<T>(true, model);
        }

        public static RestResult<T> FromError()
        {
            return new RestResult<T>(false, default);
        }

        public readonly bool TryGet(out T result)
        {
            result = _result;
            return IsSuccess;
        }
    }
}
