import { AfterViewInit, Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-pie-chart',
  imports: [],
  templateUrl: './pie-chart.html',
  styleUrl: './pie-chart.css',
})
export class PieChart implements AfterViewInit, OnDestroy {
  // Target the HTML canvas element
  @ViewChild('pieCanvas') private pieCanvas!: ElementRef<HTMLCanvasElement>;

  // Track chart instance for clean memory disposal
  private chartInstance: Chart | null = null;

  constructor() {
    // Register Chart.js modules manually
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {
    const ctx = this.pieCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chartInstance = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['Electronics', 'Clothing', 'Home Decor', 'Accessories'],
        datasets: [{
          data: [0.5, 0.3, 0.1, 0.1], // Percentage or metric count breakdown
          backgroundColor: ['#0d6efd', '#198754', '#ffc107', '#0dcaf0'], // Distinct brand colors
          borderWidth: 2,
          borderColor: '#ffffff' // White borders split the segments cleanly
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom', // Keeps layout neat by stacking keys underneath
            labels: {
              boxWidth: 12,
              padding: 15,
              font: { weight: 500 },
              color: '#212529'
            }
          }
        }
      }
    });
  }

  // Cleanup reference allocations to prevent memory leaks
  ngOnDestroy(): void {
    if (this.chartInstance) {
      this.chartInstance.destroy();
    }
  }
}
