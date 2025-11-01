using Applications.Features.Customers.Requests;
using Domains.Models.Lists.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Mappers.notes
{
    public static class CustomerNoteMapper
    {
        public static CustomerNote Create(this CustomerNoteDtoCreate source)
        {
            return new CustomerNote
            {
                 CreditAgentRefId = source.CreditAgentRefId,
                 Description = source.Description,  
                 CustomerRefId = source.CustomerRefId,
            };
            
        }


        public static CustomerNote Edit(this CustomerNoteDtoUpdate source) 
        {
            return new CustomerNote
            {
                NoteId=source.NoteId,
                CreditAgentRefId = source.CreditAgentRefId,
                Description = source.Description,
                CustomerRefId = source.CustomerRefId,
            };

        }


        public static CustomerNoteDtoUpdate Edit(this CustomerNote source)
        {
            return new CustomerNoteDtoUpdate
            {
                NoteId = source.NoteId,
                CreditAgentRefId = source.CreditAgentRefId,
                Description = source.Description,
                CustomerRefId = source.CustomerRefId
            };

        }


    }
}
