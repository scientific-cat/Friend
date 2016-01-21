using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Pricing
{
	public class DataAccessor //  : IDataReader
	{
		string _connectionString =
			"Server=localhost;" +
				"Database=Friend;" +
				"User ID=PricingAdmin;" +
				"Password=dasha;" +
				"Pooling=false";

		public void SetSource(string connection)
		{
		//	this._conn = connection;
		}

		public List<PricingData> GetPricingForProduct(string productID, string storeID)
		{
			/*
			MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
			conn.ConnectionString = this->_conn;
			conn.Open();
			*/
			List<PricingData> result = new List<PricingData> ();


		
			IDbConnection dbcon = new MySqlConnection(_connectionString);
			dbcon.Open();
		
			string sql = 
				"SELECT ShopID, ProductID, SaleDate, SalePrice, SaleVolume " +
				"from PriceLog " +
					"where ShopID = '" + storeID  + "' and ProductID = '" + productID + "' order by SalePrice asc";
			IDbCommand dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = sql;
			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) {

				PricingData d = new PricingData ();
				d.SKU = (string) reader["ProductID"];
				d.OutletName = (string)reader ["ShopID"];
				d.Price = (decimal)reader ["SalePrice"];				
				d.Volume = (decimal)reader ["SaleVolume"];
				d.PriceDate = (DateTime)reader ["SaleDate"];
				result.Add (d);
			}
			// clean up
			reader.Close();
			reader = null;
			dbcmd.Dispose();
			dbcmd = null;
			dbcon.Close();
			dbcon = null;

			return result;
		}
	}
}

