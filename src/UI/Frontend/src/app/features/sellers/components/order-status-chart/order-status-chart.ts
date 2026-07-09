import { AfterViewInit, Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-order-status-chart',
  imports: [],
  templateUrl: './order-status-chart.html',
  styleUrl: './order-status-chart.css',
})
export class OrderStatusChart implements AfterViewInit, OnDestroy {
  @ViewChild('statusCanvas') private statusCanvas!: ElementRef<HTMLCanvasElement>;
  private chartInstance: Chart | null = null;

  constructor() {
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {
    const ctx = this.statusCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chartInstance = new Chart(ctx, {
      type: 'bar',
      data: {
        // Core horizontal tracking positions array
        labels: ['Pending', 'Shipped', 'Confirmed', 'Delivered', 'Cancelled'],
        datasets: [{
          label: 'Order Count',
          data: [1, 2, 3, 4, 5],
          borderRadius: 6,

          // CHANGED: Array mapping colors directly matching labels index sequences
          backgroundColor: [
            'orange', // Pending -> Custom deep vibrant Orange
            '#82b339', // Shipped -> GreenYellow
            'purple', // Confirmed -> Purple
            'green', // Delivered -> Modern crisp Green
            'red'  // Cancelled -> Danger Red
          ]
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false }
        },
        scales: {
          x: {
            grid: { display: false },
            ticks: {
              color: '#212529',
              font: { weight: 500, size: 12 }
            }
          },
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Number of Orders (Count)',
              color: '#495057',
              font: { weight: 600, size: 12 }
            },
            ticks: {
              color: '#495057',
              stepSize: 1 // Adjusted from 50 to 1 since data points sample numbers are smaller [1, 3, 3, 4, 2]
            }
          }
        }
      }
    });
  }

  ngOnDestroy(): void {
    if (this.chartInstance) {
      this.chartInstance.destroy();
    }
  }
}

