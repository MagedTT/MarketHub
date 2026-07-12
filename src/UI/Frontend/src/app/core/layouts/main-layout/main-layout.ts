import { Component, inject } from '@angular/core';
import { Navbar } from "../../components/navbar/navbar";
import { Sidebar } from '../../components/sidebar/sidebar';
import { RouterOutlet } from '@angular/router';
import { SellerSidebar } from "../../../features/sellers/components/seller-sidebar/seller-sidebar";
import { SellerNavbar } from "../../../features/sellers/components/seller-navbar/seller-navbar";
import { SessionStoreService } from '../../services/session-store-service';

@Component({
  selector: 'app-main-layout',
  imports: [Navbar, Sidebar, RouterOutlet, SellerSidebar, SellerNavbar],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout {
  session = inject(SessionStoreService);
}
