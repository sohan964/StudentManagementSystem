using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.AttendanceDtos;
using StudentManagementSystem.Models.Components;
using System.Data;

namespace StudentManagementSystem.Repositories.AttendanceRepositories
{
    public class AttendanceRepository(IConfiguration _configuration) : IAttendanceRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        
        //takeAttendanceForSection
        public async Task<Response<object>> TakeAttendaceAsync(TakeAttendancesDto takeAttendances)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spTakeAttendanceForSection", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var attendanceTable = new DataTable();
            attendanceTable.Columns.Add("enrollment_id",  typeof(int));
            attendanceTable.Columns.Add("status", typeof(string));
            attendanceTable.Columns.Add("remarks", typeof(string));
            foreach(var attendance in takeAttendances.AttendanceInputs!)
            {
                attendanceTable.Rows.Add(attendance.Enrollment_id, attendance.Status, null);
            }

            command.Parameters.AddWithValue("@routine_id", takeAttendances.Routine_id);
            command.Parameters.AddWithValue("@session_date",  takeAttendances.Session_date);

            var attendanceListParam = command.Parameters.AddWithValue("@AttendanceList", attendanceTable);
            attendanceListParam.SqlDbType = SqlDbType.Structured;
            attendanceListParam.TypeName = "AttendanceInput";

            try
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return new Response<object>(true,"attendace add success", result);
            }catch(SqlException ex)
            {
                return new Response<object>(false, ex.Message);
            }



        }
    }
}
