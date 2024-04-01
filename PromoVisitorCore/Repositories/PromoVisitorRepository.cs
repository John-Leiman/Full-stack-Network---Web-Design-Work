using Newtonsoft.Json;
using PromoVisitor.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;

namespace PromoVisitor.Repositories
{
    public class PromoVisitorRepository
    {
        public PromoData promoSearch(string searchId)
        {
            SqlConnection sqlConn = null;
            SqlTransaction sqlTrans = null;
            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlTrans = sqlConn.BeginTransaction();

                using (SqlCommand sqlCmd = new SqlCommand("[sp_MAGANG_JOHN_promoexc]", sqlConn, sqlTrans))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("TransactionType", searchId.Contains('/') ? "SELECTlink" : "SELECTid");
                    sqlCmd.Parameters.AddWithValue("SearchId", searchId);
                    sqlCmd.Parameters.AddWithValue("Order", null);
                    sqlCmd.Parameters.AddWithValue("ErrorMessage", null);
                    sqlCmd.Parameters.AddWithValue("Method", null);
                    sqlCmd.Parameters.AddWithValue("CallPars", null);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            return JsonConvert.DeserializeObject<PromoData>(reader["PromoData"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
                throw ex;
            }
            finally
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Commit();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
            }

            return null;
        }

        public List<PromoData> promoAll(string order = null)
        {
            SqlConnection sqlConn = null;
            SqlTransaction sqlTrans = null;
            List<PromoData> dataList = new List<PromoData>();
            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlTrans = sqlConn.BeginTransaction();

                using (SqlCommand sqlCmd = new SqlCommand("[sp_MAGANG_JOHN_promoexc]", sqlConn, sqlTrans))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("TransactionType", "SELECT");
                    sqlCmd.Parameters.AddWithValue("SearchId", null);
                    sqlCmd.Parameters.AddWithValue("Order", order);
                    sqlCmd.Parameters.AddWithValue("ErrorMessage", null);
                    sqlCmd.Parameters.AddWithValue("Method", null);
                    sqlCmd.Parameters.AddWithValue("CallPars", null);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PromoData promoData = JsonConvert.DeserializeObject<PromoData>(reader["PromoData"].ToString());
                                dataList.Add((PromoData)JsonConvert.DeserializeObject<PromoData>(reader["PromoData"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
                throw ex;
            }
            finally
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Commit();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
            }

            return dataList;
        }

        public List<Category> categorize()
        {
            SqlConnection sqlConn = null;
            SqlTransaction sqlTrans = null;
            List<Category> categoryList = new List<Category>();
            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlTrans = sqlConn.BeginTransaction();

                using (SqlCommand sqlCmd = new SqlCommand("[sp_MAGANG_JOHN_promoexc]", sqlConn, sqlTrans))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("TransactionType", "CATEGORIZE");
                    sqlCmd.Parameters.AddWithValue("SearchId", null);
                    sqlCmd.Parameters.AddWithValue("Order", null);
                    sqlCmd.Parameters.AddWithValue("ErrorMessage", null);
                    sqlCmd.Parameters.AddWithValue("Method", null);
                    sqlCmd.Parameters.AddWithValue("CallPars", null);

                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Category category = new Category();
                                category.CategoryID = reader["CategoryID"].ToString();
                                category.Title = reader["Title"].ToString();
                                category.CategoryType = reader["CategoryType"].ToString();
                                categoryList.Add(category);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
                throw ex;
            }
            finally
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Commit();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
            }

            return categoryList;
        }

        public void log(int status, string errMess, string method, Parameters callPars, ElasticParameters elaPars)
        {
            SqlConnection sqlConn = null;
            SqlTransaction sqlTrans = null;
            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

                if (sqlConn.State == ConnectionState.Closed)
                    sqlConn.Open();

                sqlTrans = sqlConn.BeginTransaction();

                using (SqlCommand sqlCmd = new SqlCommand("[sp_MAGANG_JOHN_promoexc]", sqlConn, sqlTrans))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("TransactionType", "LOG");
                    sqlCmd.Parameters.AddWithValue("SearchId", null);
                    sqlCmd.Parameters.AddWithValue("Order", null);
                    sqlCmd.Parameters.AddWithValue("ErrorMessage", errMess);
                    sqlCmd.Parameters.AddWithValue("Method", method);
                    sqlCmd.Parameters.AddWithValue("CallPars", callPars == null ? JsonConvert.SerializeObject(elaPars).ToString() : JsonConvert.SerializeObject(callPars).ToString());

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Rollback();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
                throw ex;
            }
            finally
            {
                if (sqlTrans.Connection != null)
                {
                    sqlTrans.Commit();
                    sqlTrans.Dispose();
                }
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
            }
        }

        public ElasticResponse searchElastic(string bucketId, string sortBy, string ascDesc, int from, int size, string searchText, string products, string lobs, string areas)
        {
            try
            {
                string ElasticShell = ConfigurationManager.AppSettings["ElasticShell"];
                ElasticShell = ElasticShell.Replace("$sortBy", sortBy);
                ElasticShell = ElasticShell.Replace("$ascDesc", ascDesc);
                ElasticShell = ElasticShell.Replace("$from", from.ToString());
                ElasticShell = ElasticShell.Replace("$size", size.ToString());

                string searchBlock = "";
                if (!string.IsNullOrEmpty(searchText))
                {
                    searchBlock = ConfigurationManager.AppSettings["searchBlock"];
                    searchBlock = ", " + searchBlock.Replace("$searchText", searchText);
                }
                ElasticShell = ElasticShell.Replace("$searchBlock", searchBlock);

                string categoriesBlock = "";
                if (products.Any() || lobs.Any() || areas.Any())
                {
                    string[] productType = { listCats(products), "product" };
                    string[] lobType = { listCats(lobs), "lob" };
                    string[] areaType = { listCats(areas), "area" };

                    string[][] categoryTypes = { productType, lobType, areaType };

                    foreach (string[] categoryType in categoryTypes)
                    {
                        if (categoryType[0].Contains("\""))
                        {
                            string catBlock = ConfigurationManager.AppSettings["categoriesBlock"];
                            catBlock = catBlock.Replace("$catType", categoryType[1]);
                            catBlock = catBlock.Replace("$cats", categoryType[0]);
                            categoriesBlock += catBlock + ", ";
                        }
                    }
                }
                char[] trimmer = { ',', ' ' };
                ElasticShell = ElasticShell.Replace("$categoriesBlock", categoriesBlock.TrimEnd(trimmer));

                ElasticShell = ElasticShell.Replace("$bucketId", bucketId);


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ElasticEndpoint"]);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ElasticAuthorization"]);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(ElasticShell);
                }

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback
                (
                    delegate { return true; }
                );

                string jsonString;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    jsonString = streamReader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<ElasticResponse>(jsonString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string listCats(string filter)
        {
            string result = string.Empty;
            List<string> filterList = filter.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();

            try
            {
                for (var i = 0; i < filterList.Count; i++)
                {
                    result = result + '"' + filterList[i] + '"';
                    if (filterList.Count > 1 && i < (filterList.Count - 1))
                    {
                        result = result + ", ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}