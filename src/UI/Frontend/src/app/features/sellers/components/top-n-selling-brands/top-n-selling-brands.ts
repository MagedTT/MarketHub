import { AfterViewInit, Component, ElementRef, Input, OnDestroy, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';

export interface BrandData {
  name: string;
  salesCount: number;
}

@Component({
  selector: 'app-top-n-selling-brands',
  imports: [],
  templateUrl: './top-n-selling-brands.html',
  styleUrl: './top-n-selling-brands.css',
})
export class TopNSellingBrands implements AfterViewInit, OnDestroy {
  @ViewChild('brandsCanvas') private brandsCanvas!: ElementRef<HTMLCanvasElement>;
  private chartInstance: Chart | null = null;

  @Input() topN: number = 5;

  private allBrands: BrandData[] = [
    { name: 'Nike', salesCount: 1250 },
    { name: 'Adidas', salesCount: 980 },
    { name: 'Puma', salesCount: 740 },
    { name: 'Under Armour', salesCount: 520 },
    { name: 'New Balance', salesCount: 480 },
    { name: 'Reebok', salesCount: 310 },
    { name: 'Asics', salesCount: 190 }
  ];

  constructor() {
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {
    const ctx = this.brandsCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    const topBrandsDataset = [...this.allBrands]
      .sort((a, b) => b.salesCount - a.salesCount)
      .slice(0, this.topN);

    const brandLabels = topBrandsDataset.map(brand => brand.name);
    const brandValues = topBrandsDataset.map(brand => brand.salesCount);

    this.chartInstance = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: brandLabels,
        datasets: [{
          label: 'Units Sold',
          data: brandValues,
          backgroundColor: '#332b12',
          borderRadius: 6
          // CHANGED: Removed barThickness so the bars stretch naturally across the full canvas area
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
              text: 'Units Sold',
              color: '#495057',
              font: { weight: 600, size: 12 }
            },
            ticks: {
              color: '#495057'
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
