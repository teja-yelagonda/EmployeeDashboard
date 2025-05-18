using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using consoletowebapi.DBContext;
using consoletowebapi.DTO;
using consoletowebapi.Models;

namespace consoletowebapi.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<WorkerDTO, Worker>();
            CreateMap<LeaveRequestModel, LeaveRequest>();
            CreateMap<LeaveRequest, LeaveRequestModel>();
        }
    }
}
