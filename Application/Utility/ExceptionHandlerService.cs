using Application._Common.Exceptions;
using Application.Utility.Models;

namespace Application.Utility
{
    public class ExceptionHandlerService
    {
        public async Task<ExecutionResult<T>> ExecuteAsync<T>(Func<Task<T>> action)
        {
            try
            {                
                var result = await action();
                return new ExecutionResult<T>(true, null, result, ExceptionType.None, null);
            }
            catch (NotFoundException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.NotFoundException, e);
            }
            catch (BadRequestException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.BadRequestException, e);
            }
            catch (BusinessException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.BusinessException, e);
            }
        }
        public async Task<ExecutionResult> ExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
                return new ExecutionResult(true, null, ExceptionType.None, null);
            }
            catch (NotFoundException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.NotFoundException, e);
            }
            catch (BadRequestException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.BadRequestException, e);
            }
            catch (BusinessException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.BusinessException, e);
            }
        }

        public ExecutionResult<T> Execute<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return new ExecutionResult<T>(true, null, result, ExceptionType.None, null);
            }
            catch (NotFoundException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.NotFoundException, e);
            }
            catch (BadRequestException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.BadRequestException, e);
            }
            catch (BusinessException e)
            {
                return new ExecutionResult<T>(false, e.Message, default, ExceptionType.BusinessException, e);
            }
        }
        public ExecutionResult Execute(Action action)
        {
            try
            {
                action();
                return new ExecutionResult(true, null, ExceptionType.None, null);
            }
            catch (NotFoundException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.NotFoundException, e);
            }
            catch (BadRequestException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.BadRequestException, e);
            }
            catch (BusinessException e)
            {
                return new ExecutionResult(false, e.Message, ExceptionType.BusinessException, e);
            }
        }
    }
}
