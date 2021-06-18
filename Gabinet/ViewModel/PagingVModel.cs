using System;
using System.Collections;
using System.Collections.Generic;
using Gabinet.Models;

namespace Gabinet.ViewModel
{
    public class PagingVModel<T> where T : class
    {
        public IEnumerable<T> GetCollection{ get; set; }
        public PagingModel GetPaging { get; set; }


    }
}
