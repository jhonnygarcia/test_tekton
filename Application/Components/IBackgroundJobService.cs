using System.Linq.Expressions;

namespace Application.Components
{
    public interface IBackgroundJobService
    {
        void EnqueueJob(Expression<Func<Task>> expression);
        void EnqueueJob(Expression<Func<Task>> expression, TimeSpan waitTime, int retries);
        void EnqueueJob(Expression<Func<Task>> expression, int retries);
    }

}
