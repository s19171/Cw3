﻿using cw3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        public SqlServerDbService()
        {

        }
        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19171;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "select IdStudies from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);
                    com.Transaction = tran;

                    var dr = com.ExecuteReader();
                    int idstudies=0;
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return null;
                    }
                    else idstudies = (int)dr["IdStudies"];

                    com.CommandText = "select IdEnrollment from studies where IdStudy=@idstudy";
                    com.Parameters.AddWithValue("idstudy", idstudies);

                    dr = com.ExecuteReader();
                    int idenrollment = 0;
                    if (!dr.Read())
                    {
                        Random rand = new Random();
                        idenrollment = rand.Next(500,1000);
                        com.CommandText = $"INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES({idenrollment}, 1, {idstudies}, {DateTime.Now})";
                        com.ExecuteNonQuery();
                    }
                    else idenrollment = (int)dr["IdEnrollment"];

                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@Index, @Fname, @Lname, @Bdate, @IdEnrollment)";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    com.Parameters.AddWithValue("Fname", request.FirstName);
                    com.Parameters.AddWithValue("Lname", request.LastName);
                    com.Parameters.AddWithValue("Bdate", request.Birthdate);
                    com.Parameters.AddWithValue("IdEnrollment", idenrollment);
                    com.ExecuteNonQuery();

                    Enrollment ret = new Enrollment();
                    ret.IdEnrollment = idenrollment;
                    com.CommandText = $"select Semester, StartDate from studies where IdEnrollment={idenrollment}";

                    dr = com.ExecuteReader();
                    if (!dr.Read()) return null;
                    ret.Semester = Int32.Parse(dr["Semester"].ToString());
                    ret.Study = request.Studies;
                    ret.StartDate = DateTime.Parse(dr["StartDate"].ToString());

                    tran.Commit();

                    return ret;
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                }
                return null;
            }

        }

        public Enrollment PromoteStudents(int semester, string studies)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19171;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "select * from Enrollment E join Studies S on E.IdStudy=S.IdStudy where E.semester=@semester and S.Name=@studies";
                    com.Parameters.AddWithValue("semester", semester);
                    com.Parameters.AddWithValue("studies", studies);
                    com.Transaction = tran;
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return null;
                    }

                    com.CommandText = "advance_semester";
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("studies", studies);
                    com.Parameters.AddWithValue("semester", semester);

                    com.CommandText = "select IdEnrollment, Semester, E.IdStudy, StartDate from Enrollment E join Studies S on E.IdStudy=S.IdStudy where E.semester=@semester and S.Name=@studies";
                    com.Parameters.AddWithValue("semester", semester+1);
                    com.Parameters.AddWithValue("studies", studies);
                    Enrollment ret = new Enrollment();
                    dr.Close();
                    dr = com.ExecuteReader();
                    if (!dr.Read()) return null;
                    ret.IdEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                    ret.Semester = Int32.Parse(dr["Semester"].ToString());
                    ret.Study = studies;
                    ret.StartDate = DateTime.Parse(dr["StartDate"].ToString());

                    return ret;
                }
                catch(SqlException e)
                {
                    tran.Rollback();
                }
                return null;
            }
        }
    }
}
