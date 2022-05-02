using Dapper;
using Microsoft.Extensions.Configuration;
using MovieApp.Domain;
using MovieApp.Models;
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

        public async Task<int> CreateAsync(MovieVM entity)
        {
            try
            {
                string query = @"INSERT INTO Movies (Title, YearReleased, PersonsId, GenreId)
                             VALUES(@Title, @YearReleased, @PersonsId, @GenreId)";

                using (var connection = CreateConnection())
                {
                    return (await connection.ExecuteAsync(query, 
                        new { Title = entity.Title, YearReleases = entity.YearReleased, PersonsId = entity.PersonsId, GenreId = entity.GenreId }));
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
                var query = @"DELETE
                            FROM Movies
                            WHERE MovieId = @MovieId";

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
                var query = @"SELECT m.Title, m.YearReleased, p.Name AS Name, g.mGenre AS MainGenre, g.sGenre AS SubGenre
                            FROM Movies m
                            INNER JOIN Persons p
                            ON m.PersonId = p.PersonId
                            INNER JOIN Genres g
                            ON m.GenreId = g.GenreId"; 

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
                var query = @"SELECT *
                            FROM Movies
                            WHERE MovieId = @MovieId";

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
                var query = @"UPDATE Movies 
                            SET
                            Title = @Title,
                            YearReleased = @YearReleased
                            WHERE MovieId = @MovieId";

                var parameters = new DynamicParameters();
                parameters.Add("Title", entity.Title, DbType.String);
                parameters.Add("YearReleased", entity.YearReleased, DbType.Decimal);
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
