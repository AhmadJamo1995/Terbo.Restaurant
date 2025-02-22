﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public List<Meal> Meals { get; set; } = [];


        public string NameAndPrice
        {
            get
            {
                return $"{Name} - {Price} JOD";
            }
        }
    }
}
