﻿using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class GetVaccineByIdDTO
	{
        public int id { get; set; }
        public string Name { get; set; }
        public string Precautions { get; set; }
        public int DurationBetweenDoses { get; set; }
		public StatuseOfResonse Status { get; set; }


	}
}
