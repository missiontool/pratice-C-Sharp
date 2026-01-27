using AutoMapper;
using EmployeeApi.Dtos;
using EmployeeApi;

namespace EmployeeApi.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            // CreateMap<來源,目的>

            // 1. 從CreateEmployeeDto轉成Employee
            CreateMap<CreateEmployeeDto, Employee>();

            // 2. 從UpdateEmployeeDto轉成Employee
            CreateMap<UpdateEmployeeDto, Employee>();

            // 也可以反過來查詢回傳的DTO
        }
    }
}
