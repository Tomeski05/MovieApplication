using Dapper;
using Microsoft.Extensions.Configuration;
using MovieApp.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Repositories
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(IConfiguration configuration): base(configuration) { }

        public async Task<int> CreateAsync(Movies entity)
        {
            try
            {
                var query = "SET IDENTITY_INSERT Products ON " +
                            "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)" +
                            "SET IDENTITY_INSERT Products OFF";

                //var parameters = new DynamicParameters();
                //parameters.Add("Name", entity.Name, DbType.String);
                //parameters.Add("Price", entity.Price, DbType.Decimal);
                //parameters.Add("Quantity", entity.Quantity, DbType.Int32);

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, entity));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
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
