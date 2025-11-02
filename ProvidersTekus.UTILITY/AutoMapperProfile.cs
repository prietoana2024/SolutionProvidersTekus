using AutoMapper;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.UTILITY
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            #region
            CreateMap<Proveedore, Dictionary<string, object>>().
                ConvertUsing < ProveedorToDictionaryConverter > ();

            #endregion
        }
    }
}
