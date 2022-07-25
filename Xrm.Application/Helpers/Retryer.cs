using System;
using System.Linq;
using System.Threading;

namespace Xrm.Application.Helpers
{
    public class Retryer
    {
        private readonly int maxRetryCount;
        private readonly int initialDelay;
        private readonly float delayIncrementMultiplier;
        private readonly string[] filteredExceptionTexts;    

        public Retryer(int maxRetryCount, int initialDelay, float delayIncrementMultiplier, string[] filteredExceptionTexts = null)
        {
            if (maxRetryCount < 1) { throw new ArgumentOutOfRangeException(nameof(maxRetryCount)); }
            if (initialDelay < 1) { throw new ArgumentOutOfRangeException(nameof(initialDelay)); }
            if (delayIncrementMultiplier <= 0) { throw new ArgumentOutOfRangeException(nameof(delayIncrementMultiplier)); }

            this.maxRetryCount = maxRetryCount;
            this.initialDelay = initialDelay;
            this.delayIncrementMultiplier = delayIncrementMultiplier;
            this.filteredExceptionTexts = filteredExceptionTexts ?? new string[0];            
            this.filteredExceptionTexts = this.filteredExceptionTexts.Select(t => (t ?? "").ToLowerInvariant()).ToArray();           
        }

        public void ExecWithRetry(Action action, Action<Exception> onRetry = null)
        {
            ExecWithRetry(() => { action(); return ""; }, onRetry);
        }

        public TResult ExecWithRetry<TResult>(Func<TResult> func, Action<Exception> onRetry = null)
        {
            int tryNumber = 1;
            int delay = initialDelay;

            while (true)
            {
                try
                {
                    return func();                    
                }
                catch (Exception ex)
                {
                    if (tryNumber <= maxRetryCount)
                    {
                        if (filteredExceptionTexts.Length > 0)
                        {
                            string msg = (ex.Message ?? "").ToLowerInvariant();

                            if (!filteredExceptionTexts.Any(fe => msg.Contains(fe)))
                            {
                                throw;
                            }
                        }

                        Thread.Sleep(delay);

                        delay = (int)(delay * delayIncrementMultiplier);

                        tryNumber++;

                        onRetry?.Invoke(ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
