using FoodPal.Orders.DTOs;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodPal.Orders.Services
{
    public class BaseService
    {
        /// <summary>
        /// Enforces parameter checks and throws 
        /// <see cref="FoodPalBadParamsException"/>
        /// when at least one check fails
        /// <param name="paramChecksAndExceptions">array of tuples consisting of (parameter checks and exceptions)</param>
        protected void ParameterChecks((Func<bool>, Exception)[] paramChecksAndExceptions)
        {
            if (paramChecksAndExceptions != null && paramChecksAndExceptions.Any())
            {
                var paramChecks = paramChecksAndExceptions.Select(pce => pce.Item1).ToArray();
                var paramExceptions = paramChecksAndExceptions.Select(pce => pce.Item2).ToArray();
                ParameterChecks(paramChecks, paramExceptions);
            }
        }

        /// <summary>
        /// Enforces parameter checks and throws 
        /// <see cref="FoodPalBadParamsException"/>
        /// when at least one check fails
        /// </summary>
        /// <param name="paramChecks">parameter checks</param>
        /// <param name="paramExceptions">optional list of parameter exceptions</param>
        /// <remarks>When specified, the array of parameter exceptions must have the same length as the array of checks</remarks>
        protected void ParameterChecks(Func<bool>[] paramChecks, Exception[] paramExceptions = null)
        {
            // no checks to enforce
            if (paramChecks == null || !paramChecks.Any())
            {
                return;
            }

            // ignore param exceptions in case of length mismatch
            if (paramExceptions != null && paramExceptions.Length != paramChecks.Length)
            {
                paramExceptions = null;
            }

            // perform the specified parameter checks
            var checkResult = paramChecks.Select(pc => pc()).ToArray();

            if (checkResult.Any(r => !r))
            {
                List<Exception> badParamExceptions = new List<Exception>();
                if (paramExceptions != null)
                {
                    for (int i = 0; i < checkResult.Length; i++)
                    {
                        if (!checkResult[i])
                        {
                            badParamExceptions.Add(paramExceptions[i]);
                        }
                    }
                }
                var errorInfo = new ErrorInfoDTO()
                {
                    InfoType = ErrorInfoType.Error,
                    Message = $"Service operation failed due missing or invalid params",
                    Details = String.Join(",", badParamExceptions.Select(bpe => bpe.Message).ToArray())
                };

                throw new FoodPalBadParamsException(errorInfo, badParamExceptions);
            }
        }
    }
}
