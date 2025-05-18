using AutoMapper;
using consoletowebapi.DBContext;
using consoletowebapi.Models;
using consoletowebapi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IEmailService emailService,IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _emailService = emailService;
            _mapper = mapper;
        }
        public int RequestLeave(LeaveRequestModel leaveRequest)
        {
            LeaveRequest mappedRequest = _mapper.Map<LeaveRequest>(leaveRequest);
            int rowsEffected = _leaveRequestRepository.RequestLeave(mappedRequest);
            return rowsEffected;
        }
        public void LeaveProcessing()
        {
            List<LeaveRequest> pendingLeaves=_leaveRequestRepository.GetPendingLeaveRequest();
            List<LeaveRequestModel> mappedLeaves = _mapper.Map<List<LeaveRequestModel>>(pendingLeaves);
            foreach(LeaveRequestModel leave in mappedLeaves)
            {
                _emailService.SendEmailReminder(leave);
            }
        }
    }
    
}
