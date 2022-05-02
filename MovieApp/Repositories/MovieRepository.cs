using Dapper;
using Microsoft.Extensions.Configuration;
using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Repositories
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(IConfiguration configuration): base(configuration) { }

        public async Task<int> CreateAsync(Movies entity)
        {
            //try
            //{
            //    var query = "SET IDENTITY_INSERT Products ON " +
            //                "INSERT INTO Products (Title, YearReleased, Genre, Persons ) VALUES (@Title, @YearReleased, @Genre, @Persons)" +
            //                "SET IDENTITY_INSERT Products OFF";

            //    using (var connection = CreateConnection())
            //    {
            //        return (await connection.ExecuteAsync(query, entity));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message, ex);
            //}

            IDbTransaction transaction = null;

            try
            {
                using (IDbConnection connection = new SqlConnection(DbConnectionString))
                {

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    transaction = connection.BeginTransaction();

                    string sql = "INSERT INTO Master(OrdinalNumber, StringDate)" +
                                 "VALUES(@OrdinalNumber, @StringDate)";

                    connection.Execute(sql, master, transaction);

                    string sql = "INSERT INTO Child(OrdinalNumber, StringDate, Name, Amount, StartDate, EndDate)" +
                                 "VALUES(@OrdinalNumber, @StringDate, @Name, @Amount, @StartDate, @EndDate)";

                    connection.Execute(sql, child, transaction);

                    transaction.Commit();

                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (transaction != null)
                    transaction.Dispose();
            }
        }

        public async Task<int> DeleteAsync(Movies entity)
        {
            try
            {
                var query = "DELETE " +
                            "FROM Movies " +
                            "WHERE MovieId = @MovieId";

                var parameters = new DynamicParameters();
                parameters.Add("MovieId", entity.MovieId, DbType.Int64);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, parameters));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Movies>> GetAllAsync()
        {
            try
            {
                var query = "SELECT * " +
                            "FROM Movies"; 

                using (var connection = CreateConnection())
                {
                    return (await connection.QueryAsync<Movies>(query)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Movies> GetByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * " +
                            "FROM Movies " +
                            "WHERE MovieId = @MovieId";

                var parameters = new DynamicParameters();
                parameters.Add("MovieId", id, DbType.Int64);

                using (var connection = CreateConnection())
                {
                    return (await connection.QueryFirstOrDefaultAsync<Movies>(query, parameters));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(Movies entity)
        {
            try
            {
                var query = "UPDATE Movies " +
                            "SET Title = @Title, YearReleased = @YearReleased, Genre = @Genre, Persons = @Persons " +
                            "WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("Title", entity.Title, DbType.String);
                parameters.Add("YearReleased", entity.YearReleased, DbType.Decimal);
                parameters.Add("Genre", entity.Genre);
                parameters.Add("Persons", entity.Persons, DbType.String);
                parameters.Add("MovieId", entity.MovieId, DbType.Int64);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, parameters));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
