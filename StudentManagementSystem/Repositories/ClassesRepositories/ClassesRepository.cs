using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.ClassesDtos;
using StudentManagementSystem.Models.Components;
using System.Data;
namespace StudentManagementSystem.Repositories.ClassesRepositories
{
    public class ClassesRepository(IConfiguration _configuration) : IClassesRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        public async Task<Response<List<ClassesDto>>> GetClassesAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetClasses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var classes = new List<ClassesDto>();
            while(await reader.ReadAsync())
            {
                classes.Add( new ClassesDto()
                {
                    Class_id = reader.GetInt32(0),
                    Class_name = reader.GetString(1),
                    Is_secondary = reader.GetBoolean(2),
                    Description = reader.GetString(3)
                });
            }
            return new Response<List<ClassesDto>>(true, "all Classes", classes);
        }

        public async Task<Response<ClassesDto>> GetClassByIdAsync(int class_id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetClassById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@class_id", class_id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var getClass = new ClassesDto();
            while (await reader.ReadAsync())
            {
                getClass = new ClassesDto()
                {
                    Class_id = reader.GetInt32(0),
                    Class_name = reader.GetString(1),
                    Is_secondary=reader.GetBoolean(2),
                    Description = reader.GetString(3)
                };
            }
            return new Response<ClassesDto>(true, $"the {getClass.Class_name}", getClass);
        }

        ///handle ClassSubjects
        public async Task<Response<object>> AddClassSubjecsAsync(ClassSubjectsDto classSubjectsDto)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spAddClassSubjects", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var subjectTable = new DataTable();
            subjectTable.Columns.Add("subject_id", typeof(int));
            
            foreach(var subject in classSubjectsDto.Subjects_list!)
            {
                subjectTable.Rows.Add(subject);
            }

            command.Parameters.AddWithValue("@Class_id", classSubjectsDto.Class_id);
            
            var subjectListParam = command.Parameters.AddWithValue("@Subject_list", subjectTable);
            subjectListParam.SqlDbType = SqlDbType.Structured;
            subjectListParam.TypeName = "SubjectIdList";

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return new Response<object>(true, "added success", result);
        }

        //it will return subjects that under a class
        public async Task<Response<ClassSubjectsDetailsDto>> GetSubjectsByClassIdAsync(int classId)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetSubjectsByClassId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@Class_id", classId);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var classSubjectDetails = new ClassSubjectsDetailsDto();
            
            bool f1 = true;
            while(await  reader.ReadAsync())
            {
                if (f1)
                {
                    classSubjectDetails.Class_id = reader.GetInt32(0);
                    classSubjectDetails.Class_name = reader.GetString(1);
                    classSubjectDetails.Class_description = reader.GetString(2);
                    f1 = false;
                }
                classSubjectDetails.SubjectList.Add(new SubjectDetailsDto()
                {
                    Subject_id = reader.GetInt32(3),
                    Subject_code = reader.GetString(4),
                    Name = reader.GetString(5),
                    Is_theory = reader.GetBoolean(6),
                    Is_practical = reader.GetBoolean(7),
                    Default_marks = reader.GetInt32(8),
                    Is_mandatory = reader.GetBoolean(9),
                    Department_id = reader.GetInt32(10),
                    Credit_hours = reader.GetInt32(11),
                });
            }
            return new Response<ClassSubjectsDetailsDto>(true, $"Subject list under {classSubjectDetails?.Class_name}", classSubjectDetails);
        }


    }
}
