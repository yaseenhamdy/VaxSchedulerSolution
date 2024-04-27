﻿namespace VaxScheduler.API.DTOs
{
	public class AddVaccineDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }

		public int DurationBetweenDoses { get; set; }

		public string Precautions { get; set; }
		public int[] VaccinationCenterIds { get; set; }

	}
}
