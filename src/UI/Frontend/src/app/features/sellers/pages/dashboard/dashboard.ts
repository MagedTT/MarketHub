import { Component } from '@angular/core';
import { OrderStatusChart } from '../../components/order-status-chart/order-status-chart';
import { PieChart } from '../../components/pie-chart/pie-chart';
import { RatingChart } from '../../components/rating-chart/rating-chart';
import { TopNSellingBrands } from '../../components/top-n-selling-brands/top-n-selling-brands';
import { TotalX } from '../../components/total-x/total-x';

@Component({
  selector: 'app-dashboard',
  imports: [TopNSellingBrands, OrderStatusChart, PieChart, RatingChart, TotalX],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard { }
