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
            IDbTransaction transaction = null;

            try
            {
                using (var connection = CreateConnection())
                {

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    transaction = connection.BeginTransaction();

                    string query = "INSERT INTO Movies(Title, YearReleased ) VALUES (@Title, @YearReleased)";

                    return connection.Execute(query, entity, transaction);

                    string queryTwo = "INSERT INTO Persons(Title, YearReleased, Name)" +
                                 "VALUES(@Title, @YearReleased, @Name)";

                    return connection.Execute(queryTwo, entity, transaction);

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
