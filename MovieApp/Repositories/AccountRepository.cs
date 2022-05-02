using CalendarManagementApplication.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        private readonly IConfiguration _configuration;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserModel LoginUser(LoginViewModel model)
        {
            try
            {
                string query = @"SELECT *
                                From IdentityUser
                                Where Email = @Email";

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                {
                    dbConnection.Open();
                    var result = dbConnection.QueryFirstOrDefault<UserModel>(query, model);
                    return result;
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool RegisterUser(UserModel model)
        {
            try
            {
                string query = @"INSERT INTO IdentityUser
                            (FirstName, LastName, Email, Password, Role)
                             VALUES(@FirstName,@LastName,@Email,@Password,@Role)";

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                {
                    dbConnection.Open();
                    var result = dbConnection.Execute(query, model);
                    return true;
                };
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool UpdateUser(UserModel model)
        {
            try
            {
                string query = @"UPDATE IdentityUser
                             Set
                             FirstName = @FirstName,
                             LastName = @LastName
                             Where
                             Email=@Email";



                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                {
                    dbConnection.Open();
                    var result = dbConnection.Execute(query, model);
                    return true;
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangePassword(ChangePasswordModel model)
        {
            try
            {
                string query = @"UPDATE IdentityUser
                                SET
                                Password = @NewPassword
                                Where
                                Email = @Email
                                AND
                                Password = @CurrentPassword";

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                {
                    dbConnection.Open();
                    var result = dbConnection.Execute(query, model);
                    return true;
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            try
            {
                string query = @"UPDATE IdentityUser
                                SET
                                Password = @NewPassword
                                Where
                                Email = @Email";

                using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("default")))
                {
                    dbConnection.Open();
                    var result = dbConnection.Execute(query, model);
                    return true;
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
