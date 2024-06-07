/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.Framework
{
    public interface IDapperHelper
    {
        /// <summary>
        /// Method used to get list from sp without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>List of generic objects</returns>
        IEnumerable<T> GetListFromSP<T>(T genericType, string sp, string connectionString) where T : new();
        /// <summary>
        /// Method used to get list from sp
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>List of generic objects</returns>
        IEnumerable<T> GetListFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        /// <summary>
        /// Method used to get single generic class without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>single generic class</returns>
        T GetSingleFromSP<T>(T genericType, string sp, string connectionString) where T : new();
        /// <summary>
        /// Method used to get single generic class 
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>return single generic element</returns>
        public T GetSingleFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        /// <summary>
        /// method used to get first element without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>returns first generic element</returns>
        T GetFirstFromSP<T>(T genericType, string sp, string connectionString) where T : new();
        /// <summary>
        /// method used to get first element
        /// </summary>
        /// <typeparam name="T">Type of class </typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns first generic element</returns>
        T GetFirstFromSP<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        /// <summary>
        /// Get multiple generic elements 
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function</param>
        /// <returns>multiple result tuples</returns>
        Tuple<List<T1>> GetMultipleFromSP<T1>(string sp, string connectionString, object parameters,
                                       Func<SqlMapper.GridReader, IEnumerable<T1>> func1);
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
        Tuple<List<T1>, List<T2>> GetMultipleFromSP<T1, T2>(string sp, string connectionString, object parameters,
                                        Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                        Func<SqlMapper.GridReader, IEnumerable<T2>> func2);
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
        Tuple<List<T1>, List<T2>, List<T3>> GetMultipleFromSP<T1, T2, T3>(string sp, string connectionString, object parameters,
                                       Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                       Func<SqlMapper.GridReader, IEnumerable<T2>> func2,
                                       Func<SqlMapper.GridReader, IEnumerable<T3>> func3);
        /// <summary>
        /// Method used to execute no query
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        void ExecuteNonQuery(string sp, object parameters, string connectionString);
        /// <summary>
        /// Method used execute scalar
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns object</returns>
        object ExecuteScalar(string sp, string connectionString, object parameters);
        /// <summary>
        /// Method used execute scalar Async
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns task object</returns>
        Task<object> ExecuteScalarAsync(string sp, string connectionString, object parameters);
        /// <summary>
        /// Method used to execute no query async
        /// </summary>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        void ExecuteNonQueryAsync(string sp, object parameters, string connectionString);
        /// <summary>
        /// method used to get first element async
        /// </summary>
        /// <typeparam name="T">Type of class </typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>returns first generic element</returns>
        Task<T> GetFirstFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        /// <summary>
        /// method used to get first element async without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>returns first generic element</returns>
        Task<T> GetFirstFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new();
        /// <summary>
        /// Method used to get single generic class  async
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>return single generic element</returns>
        Task<T> GetSingleFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        /// <summary>
        /// Method used to get single generic class without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>single generic class</returns>
        Task<T> GetSingleFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new();

        /// <summary>
        /// Method used to get list from sp async
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <returns>List of generic objects</returns>
        Task<IEnumerable<T>> GetListFromSPAsync<T>(T genericType, string sp, string connectionString, object parameter) where T : new();
        // <summary>
        /// Method used to get list from sp without parameters
        /// </summary>
        /// <typeparam name="T">Type of class</typeparam>
        /// <param name="genericType">new instance of class</param>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <returns>List of generic objects</returns>
        Task<IEnumerable<T>> GetListFromSPAsync<T>(T genericType, string sp, string connectionString) where T : new();

        /// <summary>
        /// Get multiple generic elements Async
        /// </summary>
        /// <typeparam name="T1">Generic type class</typeparam>
        /// <param name="sp">stored procedure name</param>
        /// <param name="connectionString"> connection string</param>
        /// <param name="parameter">parameters</param>
        /// <param name="func1">Read Function</param>
        /// <returns>multiple result tuples</returns>
        Task<Tuple<List<T1>>> GetMultipleFromSPAsync<T1>(string sp, string connectionString, object parameters, Func<SqlMapper.GridReader, IEnumerable<T1>> func1);
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
        Task<Tuple<List<T1>, List<T2>>> GetMultipleFromSPAsync<T1, T2>(string sp, string connectionString, object parameters,
                                       Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                       Func<SqlMapper.GridReader, IEnumerable<T2>> func2);
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
        Task<Tuple<List<T1>, List<T2>, List<T3>>> GetMultipleFromSPAsync<T1, T2, T3>(string sp, string connectionString, object parameters,
                                       Func<SqlMapper.GridReader, IEnumerable<T1>> func1,
                                       Func<SqlMapper.GridReader, IEnumerable<T2>> func2,
                                       Func<SqlMapper.GridReader, IEnumerable<T3>> func3);
    }
}
