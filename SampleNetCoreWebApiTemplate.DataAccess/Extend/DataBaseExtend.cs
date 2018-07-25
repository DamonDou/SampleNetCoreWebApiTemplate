using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace SampleNetCoreWebApiTemplate.DataAccess.Extend
{
    public static class DataBaseExtend
    {
        /// <summary>
        ///  执行SQL查询
        /// </summary>
        /// <typeparam name="T">查询结果类型</typeparam>
        /// <param name="dbContext">数据上下文</param>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>
        ///  查询结果
        /// </returns>
        public static async Task<List<T>> SqlQueryAsync<T>(this DbContext dbContext, string sql, params SqlParameter[] parameters)
        {
            if (typeof(T).GetConstructor(new Type[0]) == null)
            {
                return await ValueListQueryAsync<T>(dbContext, sql, parameters);
            }
            else
            {
                return await ClassListQueryAsync<T>(dbContext, sql, parameters);
            }
        }

        private static async Task<List<T>> ClassListQueryAsync<T>(DbContext dbContext, string sql, params SqlParameter[] parameters)
        {
            var result = new List<T>();
            var modelProperties = typeof(T).GetProperties();

            using (var connection = dbContext.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Any())
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = Activator.CreateInstance(typeof(T));

                            foreach (var property in modelProperties)
                            {
                                var index = reader.GetOrdinal(property.Name);

                                var fieldValue = reader.GetValue(index);

                                property.SetValue(model, fieldValue);
                            }

                            result.Add((T)model);
                        }
                    }
                }
            }
            return result;
        }

        private static async Task<List<T>> ValueListQueryAsync<T>(DbContext dbContext, string sql, params SqlParameter[] parameters)
        {
            var result = new List<T>();

            using (var connection = dbContext.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    if (parameters != null && parameters.Any())
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add((T)reader.GetValue(0));
                        }
                    }
                }
            }
            return result;
        }
    }
}