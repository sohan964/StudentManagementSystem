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
                if (result != null && result.ToString()!.Contains("Attendance already taken"))
                {
                    return new Response<object>(false, "Attendance already taken", result);
                }
                return new Response<object>(true,"attendace add success", result);
            }catch(SqlException ex)
            {
                return new Response<object>(false, ex.Message);
            }

        }

        public async Task<Response<List<GetAttendancesDto>>> GetAttendanceSummaryAsync(int? year_id, int? class_id, int? section_id, int? subject_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetAttendanceSummary", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            command.Parameters.AddWithValue("@year_id", year_id);
            command.Parameters.AddWithValue("@class_id", class_id);
            command.Parameters.AddWithValue("@section_id", section_id);
            command.Parameters.AddWithValue("@subject_id", subject_id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var attendanceSummary = new List<GetAttendancesDto>();
            while(await reader.ReadAsync())
            {
                attendanceSummary.Add(new GetAttendancesDto()
                {
                    Enrollment_id = reader.GetInt32(0),
                    Student_id = reader.GetInt32(1),
                    Student_number = reader.GetString(2),
                    Year_id = reader.GetInt32(3),
                    Class_id = reader.GetInt32(4),
                    Section_id = reader.GetInt32(5),
                    Subject_id = reader.GetInt32(6),
                    First_name = reader.GetString(7),
                    Last_name = reader.GetString(8),
                    Total_classes = reader.GetInt32(9),
                    Total_present = reader.GetInt32(10),
                    Total_absent = reader.GetInt32(11),
                    Attendance_percentage = (int)reader.GetDecimal(12),
                });
            }
            return new Response<List<GetAttendancesDto>>(true, "All the Attendance summary of this call", attendanceSummary);
            
        }

        //get student subject attendance
        public async Task<Response<GetAttendanceDetailsDto>> GetAttendanceDetailsAsync(int enrollment_id, int year_id, int subject_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetStudentAttendanceDetails", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@enrollment_id", enrollment_id);
            command.Parameters.AddWithValue("@year_id", year_id);
            command.Parameters.AddWithValue("@subject_id", subject_id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var attendanceList = new GetAttendanceDetailsDto();
            var studentSubjectAttendance = new StudentSubjectAttendance();
            bool f1 = true;
            while(await reader.ReadAsync())
            {
                 studentSubjectAttendance = new StudentSubjectAttendance()
                 {
                      Record_id = reader.GetInt32(0),
                      Session_id = reader.GetInt32(1),
                      Session_date = DateOnly.FromDateTime(reader.GetDateTime(2)),
                      Status = reader.GetString(3) 
                 };
                attendanceList.studentSubjectAttendances.Add(studentSubjectAttendance);

                if(f1)
                {
                    attendanceList.Subject_id = reader.GetInt32(7);
                    attendanceList.Class_id = reader.GetInt32(8);
                    attendanceList.Section_id = reader.GetInt32(9);
                    attendanceList.Teacher_id = reader.GetInt32(10);
                    attendanceList.Student_id = reader.GetInt32(11);
                    attendanceList.Student_name = reader.GetString(13) + reader.GetString(14);
                    f1 = false;
                }
            }

            return new Response<GetAttendanceDetailsDto>(true, "Attendance Details of the subject for this student", attendanceList);
        }

        public async Task<Response<object>> UpdateAttendanceByRecordAsync(int record_id, string status)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spUpdateAttendanceRecord", connection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@record_id", record_id);
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "Updated success full", result);
            
        }
    }
}
