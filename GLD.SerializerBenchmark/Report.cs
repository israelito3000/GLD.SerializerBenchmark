﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GLD.SerializerBenchmark
{
    internal static class Report
    {
        public static void AllResults(Dictionary<string, Measurements[]> measurements, List<string> aborts)
        {
            Header();
            foreach (var oneTestMeasurments in measurements)
                SingleResult(oneTestMeasurments);
            Aborts(aborts);
        }

        private static void Aborts(List<string> aborts)
        {
            if (aborts.Count <= 1) return;

            const string abortHeader = "\nABORTS: some serializers throw exceptions *********************\n";
            Console.WriteLine(abortHeader);
            Trace.WriteLine(abortHeader);

            aborts = aborts.Select(abort => abort).Distinct().ToList();
            foreach (var abort in aborts)
            {
                Console.WriteLine(abort);
                Trace.WriteLine(abort);
            }
        }

        public static void TimeAndDocument(string name, long timeTicks, string document)
        {
            Trace.WriteLine(name + ": " + timeTicks + " ticks Document: " + document);
        }

        public static void Errors(List<string> errors)
        {
            if (errors.Count <= 1) return;
            foreach (var error in errors)
            {
                Trace.WriteLine(error);
                //Console.WriteLine(error);
            }
        }

        public static void Repetitions(int repetitions)
        {
            var str = "Repetitions: " + repetitions;
            Console.WriteLine(str);
            Trace.WriteLine(str);
        }

        public static void TestDataHeader(string key)
        {
            var name = "\nTest Data: " + key + " ";
            var str = name + new string('>', 80 - name.Length);
            Console.WriteLine(str);
            Trace.WriteLine(str);
        }

        private static void SingleResult(KeyValuePair<string, Measurements[]> oneTestMeasurements)
        {
            var report =
                //string.Format("{0, -20} {1,7:N0} {2,7:N0} {3,6:N0} {4,9:N0} {5,10:N0} {6,6:N0}",
                string.Format("{0, -20} {1,7:N0} {2,7:N0} {3,6:N0} {4,10:N0} {5,6:N0}",
                    oneTestMeasurements.Key,
                    //AverageTime(oneTestMeasurements.Value, 20),
                    AverageTime(oneTestMeasurements.Value, 10),
                    AverageTime(oneTestMeasurements.Value),
                    MinTime(oneTestMeasurements.Value),
                    //P99Time(oneTestMeasurements.Value),
                    MaxTime(oneTestMeasurements.Value),
                    AverageSize(oneTestMeasurements.Value));

            Console.WriteLine(report);
            Trace.WriteLine(report);
        }

        private static void Header()
        {
            var header = "\nSerializer:    Time: Avg-90%   -100%    Min      Max  Size: Avg\n"
                         + new string('=', 64);
            Console.WriteLine(header);
            Trace.WriteLine(header);
        }

        private static double P99Time(Measurements[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return BottomPercent(measurements, 1).Select(m => m.Time).LastOrDefault();
        }

        private static double MaxTime(Measurements[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return measurements.Max(m => m.Time);
        }

        private static double MinTime(Measurements[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return measurements.Min(m => m.Time);
        }

        private static IEnumerable<Measurements> BottomPercent(Measurements[] measurements, int discardedPercent)
        {
            if (discardedPercent == 0) return measurements;
            var take = (int) Math.Round(measurements.Length*(100 - discardedPercent)/100.0);
            return measurements.OrderBy(m => m.Time).Take(take);
        }

        private static double AverageTime(Measurements[] measurements, int discardedPercent = 0)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return BottomPercent(measurements, discardedPercent).Average(m => m.Time);
        }

        private static int AverageSize(Measurements[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return (int) measurements.Average(m => m.Size);
        }

     }
}