using Microsoft.Data.SqlClient;
using StudentManagementSystem.Models.Components;
using StudentManagementSystem.Models.DaysDtos;
using System.Data;
namespace StudentManagementSystem.Repositories.WeeklyDaysRepositories
{
    public class WeeklyDaysRepository(IConfiguration _configuration) : IWeeklyDaysRepository
    {
        private string connectionString = _configuration.GetConnectionString("DefaultConnection")!;

        //get days
        public async Task<Response<List<DayDto>>> GetWeeklyDaysAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetDays", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var dayList = new List<DayDto>();
            while( await reader.ReadAsync())
            {
                dayList.Add(new DayDto()
                {
                    Day_id = reader.GetInt32(0),
                    Day_name = reader.GetString(1),
                    Is_school_open = reader.GetBoolean(2)
                });
            }
            return new Response<List<DayDto>>(true, "all days", dayList);
        }

        public async Task<Response<List<TimeSlotsDto>>> GetTimeSlotsAsync()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("spGetClassSlots", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            var slotsList = new List<TimeSlotsDto>();
            while( await reader.ReadAsync())
            {
                slotsList.Add(new TimeSlotsDto() 
                {
                    Slot_id = reader.GetInt32(0),
                    Slot_number = reader.GetInt32(1),
                    Start_time = reader.GetString(2),
                    End_time = reader.GetString(3)
                });
            }

            return new Response<List<TimeSlotsDto>>(true, "all time Slots", slotsList);
        }

    }
}
