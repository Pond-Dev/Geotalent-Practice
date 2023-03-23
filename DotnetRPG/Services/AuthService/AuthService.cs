using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using DotnetRpg.Services.Settings;
using Microsoft.Data.SqlClient;
using System.Data;
using Serilog;

namespace DotnetRpg.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly JwtSettings _jwtSettings;
        public AuthService(ISettingsSingletonService settingsSingletonService)
        {
            _connectionStrings = settingsSingletonService.ConnectionStrings;
            _jwtSettings = settingsSingletonService.JwtSettings;
            
        }
        public bool ExistUser(string username)
        {
            bool exist = false ;
             using (SqlConnection conn = new (_connectionStrings.DotNetRpgDatabase))
            {
                conn.Open();
               try
               {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "[dot].[EXIST_USER]";
                        command.Parameters.AddWithValue("@USERNAME",username);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            int found = reader.GetFieldValue<int>("Found");
                            exist = found > 0 ;                            
                        }
                         reader.Close();
                    }
               }
               finally
               {
                    conn.Close();
               } 
                
            }

            return exist;
        }

        public string Login(string username, string password)
        {
             string? accessToken = null ;
             string? dbPasswordHash = null;
             UserProfile? user = null ;
             using (SqlConnection conn = new (_connectionStrings.DotNetRpgDatabase))
            {
                conn.Open();
               try
               {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "[dot].[FIND_USER]";
                        command.Parameters.AddWithValue("@USERNAME",username);

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            dbPasswordHash = reader.GetFieldValue<string>("PasswordHash");
                            user = new UserProfile()
                            {
                                Id =reader.GetFieldValue<int>("Id"),
                                UserName = reader.GetFieldValue<string>("Username"),
                                PasswordHash = dbPasswordHash
                            };
                            
                        }
                         reader.Close();
                    }
               }
               finally
               {
                    conn.Close();
               } 
                
            }
            if(dbPasswordHash != null)
            {
               bool valid = AuthUtilities.VerifyPasswordHash(password,dbPasswordHash);
               if(valid)
               {
                accessToken = AuthUtilities.GenerateToken(user,_jwtSettings); 
               }
            }
            

            return accessToken;
        }

        public int Register(string username, string password)
        {
            int userId = -1;

            try {
               string hashPassword =  AuthUtilities.CreateHashPassword(password);
               using(SqlConnection conn = new(_connectionStrings.DotNetRpgDatabase))
               {
                conn.Open();
                var transaction = conn.BeginTransaction();
                try
                {
                    using(SqlCommand command = conn.CreateCommand())
                    {
                        command.Transaction= transaction ;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "[dot].[CREATE_NEW_USER]";
                        command.Parameters.AddWithValue("@USERNAME",username);
                        command.Parameters.AddWithValue("@HASH_PASSWORD",hashPassword);
                        command.Parameters.Add(new()
                        {
                            Direction = System.Data.ParameterDirection.Output,
                            ParameterName = "@USER_ID",
                            DbType = System.Data.DbType.Int32
                        });


                        command.ExecuteNonQuery();
                        int newId = Convert.ToInt32(command.Parameters["@USER_ID"].Value);
                        if(newId > 0) 
                        {
                            transaction.Commit();
                            userId = newId;
                        } else {
                            transaction.Rollback();
                        }

                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex,"Register Error");
                    if(conn!= null)
                    {
                        transaction.Rollback();
                    }
                }
                finally
                {
                    conn.Close();
                }
               }

            }
            catch(System.Exception){
                throw;
            }

            return userId ;
        }
    }
}