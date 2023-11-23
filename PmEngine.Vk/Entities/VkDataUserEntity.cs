﻿using Microsoft.EntityFrameworkCore;
using PmEngine.Core.Entities;
using PmEngine.Core.Interfaces;

namespace PmEngine.Vk.Entities
{
    [PrimaryKey("VkId")]
    public class VkDataUserEntity : IDataEntity
    {
        public virtual UserEntity Owner { get; set; }
        public long VkId { get; set; }
    }
}