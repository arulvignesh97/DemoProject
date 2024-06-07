/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CTS.Applens.Framework
{
    /// <summary>
    /// Generic dappper methods implementation
    /// </summary>
    public class DapperHelper : IDapperHelper
    {
        private SqlConnectionStringBuilder GetSqlConnectionStringBuilder(string connectionString)
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder(connectionString);
            connectionBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;

            return connectionBuilder;
        }


        /// <summary>
        /// Method used to get list from sp without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>List of generic objects</returns>
        public IEnumerable<T> GetListFromSP<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return GetListFromSP<T>(genericType, sp, connectionString, null);
        }
        // <summary>
        /// Method used to get list from sp without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>List of generic objects</returns>
        public async Task<IEnumerable<T>> GetListFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return await GetListFromSPAsync<T>(genericType, sp, connectionString, null);
        }
        /// <summary>
        /// Method used to get list from sp
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>List of generic objects</returns>
        public IEnumerable<T> GetListFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection.Query<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// Method used to get list from sp async
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>List of generic objects</returns>
        public async Task<IEnumerable<T>> GetListFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return await connection.QueryAsync<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// Method used to get single generic class without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>single generic class</returns>
        public T GetSingleFromSP<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return GetSingleFromSP<T>(genericType, sp, connectionString, null);
        }
        /// <summary>
        /// Method used to get single generic class without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>single generic class</returns>
        public async Task<T> GetSingleFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return await GetSingleFromSPAsync<T>(genericType, sp, connectionString, null);
        }
        /// <summary>
        /// Method used to get single generic class 
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>return single generic element</returns>
        public T GetSingleFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection.QuerySingleOrDefault<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// Method used to get single generic class  async
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>return single generic element</returns>
        public async Task<T> GetSingleFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return await connection.QuerySingleOrDefaultAsync<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// method used to get first element without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>returns first generic element</returns>
        public T GetFirstFromSP<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return GetFirstFromSP<T>(genericType, sp, connectionString, null);
        }
        /// <summary>
        /// method used to get first element async without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>returns first generic element</returns>
        public async Task<T> GetFirstFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new()
        {
            return await GetFirstFromSPAsync<T>(genericType, sp, connectionString, null);
        }
        /// <summary>
        /// method used to get first element
        /// </summary>
        /// <typeparam name="T">Type of class </typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns first generic element</returns>
        public T GetFirstFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection.QueryFirstOrDefault<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// method used to get first element async
        /// </summary>
        /// <typeparam name="T">Type of class </typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns first generic element</returns>
        public async Task<T> GetFirstFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new()
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return await connection.QueryFirstOrDefaultAsync<T>(sp, parameter, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
        }
        /// <summary>
        /// Get multiple generic elements 
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function</param>
        /// <returns>multiple result tuples</returns>
        public Tuple<List<T1>> GetMultipleFromSP<T1>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1)
        {
            var objs = GetMultipleQuery(sp, connectionString, parameters, func1);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>());
            }
            return Tuple.Create(objs[0] as List<T1>);
        }

        /// <summary>
        /// Get multiple generic elements Async
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function</param>
        /// <returns>multiple result tuples</returns>
        public async Task<Tuple<List<T1>>> GetMultipleFromSPAsync<T1>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1)
        {
            var objs = await GetMultipleQueryAsync(sp, connectionString, parameters, func1);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>());
            }
            return Tuple.Create(objs[0] as List<T1>);
        }
        /// <summary>
        /// Get multiple generic elements 
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <typeparam name="T2">Generic type class 2</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function 1 </param>
        /// <param name="func2">Read function 2</param>
        /// <returns>multiple result tuples with 2 items</returns>
        public Tuple<List<T1>, List<T2>> GetMultipleFromSP<T1, T2>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<SqlMapper.GridReader, IEnumerable<T2>> func2)
        {
            var objs = GetMultipleQuery(sp, connectionString, parameters, func1, func2);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>(), new List<T2>());
            }
            return Tuple.Create(objs[0] as List<T1>, objs[1] as List<T2>);
        }

        /// <summary>
        /// Get multiple generic elements Async
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <typeparam name="T2">Generic type class 2</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function 1 </param>
        /// <param name="func2">Read function 2</param>
        /// <returns>multiple result tuples with 2 items</returns>
        public async Task<Tuple<List<T1>, List<T2>>> GetMultipleFromSPAsync<T1, T2>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<SqlMapper.GridReader, IEnumerable<T2>> func2)
        {
            var objs = await GetMultipleQueryAsync(sp, connectionString, parameters, func1, func2);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>(), new List<T2>());
            }
            return Tuple.Create(objs[0] as List<T1>, objs[1] as List<T2>);
        }
        /// <summary>
        /// Get multiple generic elements 
        /// </summary>
        /// <typeparam name="T1">Generic type class 1</typeparam>
        /// <typeparam name="T2">Generic type class 2</typeparam>
        /// <typeparam name="T3">Generic type class 3</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function 1 </param>
        /// <param name="func2">Read function 2</param>
        /// <param name="func3">Read function 3</param>
        /// <returns>multiple result tuples with 3 items</returns>
        public Tuple<List<T1>, List<T2>, List<T3>> GetMultipleFromSP<T1, T2, T3>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<SqlMapper.GridReader, IEnumerable<T2>> func2,
                                        Func<SqlMapper.GridReader, IEnumerable<T3>> func3)
        {
            var objs = GetMultipleQuery(sp, connectionString, parameters, func1, func2, func3);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>(), new List<T2>(), new List<T3>());
            }
            return Tuple.Create(objs[0] as List<T1>, objs[1] as List<T2>, objs[2] as List<T3>);
        }
        /// <summary>
        /// Get multiple generic elements async
        /// </summary>
        /// <typeparam name="T1">Generic type class 1</typeparam>
        /// <typeparam name="T2">Generic type class 2</typeparam>
        /// <typeparam name="T3">Generic type class 3</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function 1 </param>
        /// <param name="func2">Read function 2</param>
        /// <param name="func3">Read function 3</param>
        /// <returns>multiple result tuples with 3 items</returns>
        public async Task<Tuple<List<T1>, List<T2>, List<T3>>> GetMultipleFromSPAsync<T1, T2, T3>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<SqlMapper.GridReader, IEnumerable<T2>> func2,
                                        Func<SqlMapper.GridReader, IEnumerable<T3>> func3)
        {
            var objs = await GetMultipleQueryAsync(sp, connectionString, parameters, func1, func2, func3);
            if (objs.Count == 0)
            {
                return Tuple.Create(new List<T1>(), new List<T2>(), new List<T3>());
            }
            return Tuple.Create(objs[0] as List<T1>, objs[1] as List<T2>, objs[2] as List<T3>);
        }

        private List<object> GetMultipleQuery(string sp, string connectionString, object parameters, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using (var connection = new SqlConnection(connectionBuilder.ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var gridReader = connection.QueryMultiple(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }

            return returnResults;
        }

        private async Task<List<object>> GetMultipleQueryAsync(string sp, string connectionString, object parameters, params Func<SqlMapper.GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using (var connection = new SqlConnection(connectionBuilder.ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var gridReader = await connection.QueryMultipleAsync(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: connection.ConnectionTimeout);
                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }

            return returnResults;
        }
        /// <summary>
        /// Method used to execute no query
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        public void ExecuteNonQuery(string sp, object parameters, string connectionString)
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            var command = new CommandDefinition(sp, parameters,
                commandTimeout: connection.ConnectionTimeout,
                commandType: CommandType.StoredProcedure);
            connection.Execute(command);
        }
        /// <summary>
        /// Method used to execute no query async
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        public async void ExecuteNonQueryAsync(string sp, object parameters, string connectionString)
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            var command = new CommandDefinition(sp, parameters,
                commandTimeout: connection.ConnectionTimeout,
                commandType: CommandType.StoredProcedure);
            await connection.ExecuteAsync(command);
        }
        /// <summary>
        /// Method used execute scalar
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns object</returns>
        public object ExecuteScalar(string sp, string connectionString, object parameters)
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using SqlConnection connection = new SqlConnection(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            var command = new CommandDefinition(sp, parameters,
                commandTimeout: connection.ConnectionTimeout,
                commandType: CommandType.StoredProcedure);
            return connection.ExecuteScalar(command);
        }
        /// <summary>
        /// Method used execute scalar Async
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns task object</returns>
        public async Task<object> ExecuteScalarAsync(string sp, string connectionString, object parameters)
        {
            SqlConnectionStringBuilder connectionBuilder = GetSqlConnectionStringBuilder(connectionString);
            using SqlConnection connection = new(connectionBuilder.ConnectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            var command = new CommandDefinition(sp, parameters,
                commandTimeout: connection.ConnectionTimeout,
                commandType: CommandType.StoredProcedure);
            return await connection.ExecuteScalarAsync(command);
        }
    }
}
