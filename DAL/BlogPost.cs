using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//请先添加引用
namespace Maticsoft.DAL
{
	/// <summary>
	/// 数据访问类BlogPost。
	/// </summary>
	public class BlogPost
	{
		public BlogPost()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "BlogPost"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from BlogPost");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Maticsoft.Model.BlogPost model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BlogPost(");
			strSql.Append("OwnerId,OwnerType,OwnerUrl,AppId,SysCategoryId,CreatorId,CreatorUrl,CategoryId,Title,Content,Abstract,Pic,CommentCondition,Hits,IsPic,IsPick,IsTop,Replies,SaveStatus,AccessStatus,Ip,Created)");
			strSql.Append(" values (");
			strSql.Append("@OwnerId,@OwnerType,@OwnerUrl,@AppId,@SysCategoryId,@CreatorId,@CreatorUrl,@CategoryId,@Title,@Content,@Abstract,@Pic,@CommentCondition,@Hits,@IsPic,@IsPick,@IsTop,@Replies,@SaveStatus,@AccessStatus,@Ip,@Created)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@OwnerType", SqlDbType.NVarChar,250),
					new SqlParameter("@OwnerUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@AppId", SqlDbType.Int,4),
					new SqlParameter("@SysCategoryId", SqlDbType.Int,4),
					new SqlParameter("@CreatorId", SqlDbType.Int,4),
					new SqlParameter("@CreatorUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,250),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Abstract", SqlDbType.NText),
					new SqlParameter("@Pic", SqlDbType.NVarChar,250),
					new SqlParameter("@CommentCondition", SqlDbType.TinyInt,1),
					new SqlParameter("@Hits", SqlDbType.Int,4),
					new SqlParameter("@IsPic", SqlDbType.TinyInt,1),
					new SqlParameter("@IsPick", SqlDbType.TinyInt,1),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@Replies", SqlDbType.Int,4),
					new SqlParameter("@SaveStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@AccessStatus", SqlDbType.Int,4),
					new SqlParameter("@Ip", SqlDbType.NVarChar,40),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.OwnerId;
			parameters[1].Value = model.OwnerType;
			parameters[2].Value = model.OwnerUrl;
			parameters[3].Value = model.AppId;
			parameters[4].Value = model.SysCategoryId;
			parameters[5].Value = model.CreatorId;
			parameters[6].Value = model.CreatorUrl;
			parameters[7].Value = model.CategoryId;
			parameters[8].Value = model.Title;
			parameters[9].Value = model.Content;
			parameters[10].Value = model.Abstract;
			parameters[11].Value = model.Pic;
			parameters[12].Value = model.CommentCondition;
			parameters[13].Value = model.Hits;
			parameters[14].Value = model.IsPic;
			parameters[15].Value = model.IsPick;
			parameters[16].Value = model.IsTop;
			parameters[17].Value = model.Replies;
			parameters[18].Value = model.SaveStatus;
			parameters[19].Value = model.AccessStatus;
			parameters[20].Value = model.Ip;
			parameters[21].Value = model.Created;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 1;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Maticsoft.Model.BlogPost model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BlogPost set ");
			strSql.Append("OwnerId=@OwnerId,");
			strSql.Append("OwnerType=@OwnerType,");
			strSql.Append("OwnerUrl=@OwnerUrl,");
			strSql.Append("AppId=@AppId,");
			strSql.Append("SysCategoryId=@SysCategoryId,");
			strSql.Append("CreatorId=@CreatorId,");
			strSql.Append("CreatorUrl=@CreatorUrl,");
			strSql.Append("CategoryId=@CategoryId,");
			strSql.Append("Title=@Title,");
			strSql.Append("Content=@Content,");
			strSql.Append("Abstract=@Abstract,");
			strSql.Append("Pic=@Pic,");
			strSql.Append("CommentCondition=@CommentCondition,");
			strSql.Append("Hits=@Hits,");
			strSql.Append("IsPic=@IsPic,");
			strSql.Append("IsPick=@IsPick,");
			strSql.Append("IsTop=@IsTop,");
			strSql.Append("Replies=@Replies,");
			strSql.Append("SaveStatus=@SaveStatus,");
			strSql.Append("AccessStatus=@AccessStatus,");
			strSql.Append("Ip=@Ip,");
			strSql.Append("Created=@Created");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@OwnerType", SqlDbType.NVarChar,250),
					new SqlParameter("@OwnerUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@AppId", SqlDbType.Int,4),
					new SqlParameter("@SysCategoryId", SqlDbType.Int,4),
					new SqlParameter("@CreatorId", SqlDbType.Int,4),
					new SqlParameter("@CreatorUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,250),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Abstract", SqlDbType.NText),
					new SqlParameter("@Pic", SqlDbType.NVarChar,250),
					new SqlParameter("@CommentCondition", SqlDbType.TinyInt,1),
					new SqlParameter("@Hits", SqlDbType.Int,4),
					new SqlParameter("@IsPic", SqlDbType.TinyInt,1),
					new SqlParameter("@IsPick", SqlDbType.TinyInt,1),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@Replies", SqlDbType.Int,4),
					new SqlParameter("@SaveStatus", SqlDbType.TinyInt,1),
					new SqlParameter("@AccessStatus", SqlDbType.Int,4),
					new SqlParameter("@Ip", SqlDbType.NVarChar,40),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.OwnerId;
			parameters[2].Value = model.OwnerType;
			parameters[3].Value = model.OwnerUrl;
			parameters[4].Value = model.AppId;
			parameters[5].Value = model.SysCategoryId;
			parameters[6].Value = model.CreatorId;
			parameters[7].Value = model.CreatorUrl;
			parameters[8].Value = model.CategoryId;
			parameters[9].Value = model.Title;
			parameters[10].Value = model.Content;
			parameters[11].Value = model.Abstract;
			parameters[12].Value = model.Pic;
			parameters[13].Value = model.CommentCondition;
			parameters[14].Value = model.Hits;
			parameters[15].Value = model.IsPic;
			parameters[16].Value = model.IsPick;
			parameters[17].Value = model.IsTop;
			parameters[18].Value = model.Replies;
			parameters[19].Value = model.SaveStatus;
			parameters[20].Value = model.AccessStatus;
			parameters[21].Value = model.Ip;
			parameters[22].Value = model.Created;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BlogPost ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.BlogPost GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,OwnerId,OwnerType,OwnerUrl,AppId,SysCategoryId,CreatorId,CreatorUrl,CategoryId,Title,Content,Abstract,Pic,CommentCondition,Hits,IsPic,IsPick,IsTop,Replies,SaveStatus,AccessStatus,Ip,Created from BlogPost ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			Maticsoft.Model.BlogPost model=new Maticsoft.Model.BlogPost();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OwnerId"].ToString()!="")
				{
					model.OwnerId=int.Parse(ds.Tables[0].Rows[0]["OwnerId"].ToString());
				}
				model.OwnerType=ds.Tables[0].Rows[0]["OwnerType"].ToString();
				model.OwnerUrl=ds.Tables[0].Rows[0]["OwnerUrl"].ToString();
				if(ds.Tables[0].Rows[0]["AppId"].ToString()!="")
				{
					model.AppId=int.Parse(ds.Tables[0].Rows[0]["AppId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SysCategoryId"].ToString()!="")
				{
					model.SysCategoryId=int.Parse(ds.Tables[0].Rows[0]["SysCategoryId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CreatorId"].ToString()!="")
				{
					model.CreatorId=int.Parse(ds.Tables[0].Rows[0]["CreatorId"].ToString());
				}
				model.CreatorUrl=ds.Tables[0].Rows[0]["CreatorUrl"].ToString();
				if(ds.Tables[0].Rows[0]["CategoryId"].ToString()!="")
				{
					model.CategoryId=int.Parse(ds.Tables[0].Rows[0]["CategoryId"].ToString());
				}
				model.Title=ds.Tables[0].Rows[0]["Title"].ToString();
				model.Content=ds.Tables[0].Rows[0]["Content"].ToString();
				model.Abstract=ds.Tables[0].Rows[0]["Abstract"].ToString();
				model.Pic=ds.Tables[0].Rows[0]["Pic"].ToString();
				if(ds.Tables[0].Rows[0]["CommentCondition"].ToString()!="")
				{
					model.CommentCondition=int.Parse(ds.Tables[0].Rows[0]["CommentCondition"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Hits"].ToString()!="")
				{
					model.Hits=int.Parse(ds.Tables[0].Rows[0]["Hits"].ToString());
				}
				if(ds.Tables[0].Rows[0]["IsPic"].ToString()!="")
				{
					model.IsPic=int.Parse(ds.Tables[0].Rows[0]["IsPic"].ToString());
				}
				if(ds.Tables[0].Rows[0]["IsPick"].ToString()!="")
				{
					model.IsPick=int.Parse(ds.Tables[0].Rows[0]["IsPick"].ToString());
				}
				if(ds.Tables[0].Rows[0]["IsTop"].ToString()!="")
				{
					model.IsTop=int.Parse(ds.Tables[0].Rows[0]["IsTop"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Replies"].ToString()!="")
				{
					model.Replies=int.Parse(ds.Tables[0].Rows[0]["Replies"].ToString());
				}
				if(ds.Tables[0].Rows[0]["SaveStatus"].ToString()!="")
				{
					model.SaveStatus=int.Parse(ds.Tables[0].Rows[0]["SaveStatus"].ToString());
				}
				if(ds.Tables[0].Rows[0]["AccessStatus"].ToString()!="")
				{
					model.AccessStatus=int.Parse(ds.Tables[0].Rows[0]["AccessStatus"].ToString());
				}
				model.Ip=ds.Tables[0].Rows[0]["Ip"].ToString();
				if(ds.Tables[0].Rows[0]["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,OwnerId,OwnerType,OwnerUrl,AppId,SysCategoryId,CreatorId,CreatorUrl,CategoryId,Title,Content,Abstract,Pic,CommentCondition,Hits,IsPic,IsPick,IsTop,Replies,SaveStatus,AccessStatus,Ip,Created ");
			strSql.Append(" FROM BlogPost ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Id,OwnerId,OwnerType,OwnerUrl,AppId,SysCategoryId,CreatorId,CreatorUrl,CategoryId,Title,Content,Abstract,Pic,CommentCondition,Hits,IsPic,IsPick,IsTop,Replies,SaveStatus,AccessStatus,Ip,Created ");
			strSql.Append(" FROM BlogPost ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "BlogPost";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  成员方法
	}
}

