import { AfterViewInit, Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-rating-chart',
  imports: [],
  templateUrl: './rating-chart.html',
  styleUrl: './rating-chart.css',
})
export class RatingChart implements AfterViewInit, OnDestroy {
  @ViewChild('ratingCanvas') private ratingCanvas!: ElementRef<HTMLCanvasElement>;
  private chartInstance: Chart | null = null;

  constructor() {
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {
    const ctx = this.ratingCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chartInstance = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['1 Star', '2 Stars', '3 Stars', '4 Stars', '5 Stars'],
        datasets: [{
          label: 'Reviews Count',
          data: [1, 20, 20, 10, 4],
          // backgroundColor: '#332b12',
          backgroundColor: [
            '#7ba838', // Pending -> Custom deep vibrant Orange
            '#7a9e45', // Shipped -> GreenYellow
            '#5d713e', // Confirmed -> Purple
            '#4a5934', // Delivered -> Modern crisp Green
            '#434d34'  // Cancelled -> Danger Red
          ],
          borderRadius: 6
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
              font: { weight: 500 }
            }
          },
          y: {
            beginAtZero: true,
            title: {
              display: true,
              text: 'Number of Reviews',
              color: '#495057',
              font: { weight: 600, size: 12 }
            },
            ticks: { stepSize: 25 }
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
