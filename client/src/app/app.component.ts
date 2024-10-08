import { Component,OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from "./navbar/navbar.component";
import { HttpClient, HttpClientModule } from '@angular/common/http';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent,HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit
{
  title = 'eShopping';
  constructor(private http:HttpClient){}
  ngOnInit():void{
   this.http.get('http://localhost:9010/Catalog/GetProductsByBrandName/Adidas').subscribe({
    next:response => console.log(response),
    error: error => console.log(error),
    complete:() => {
      console.log("Event catalog API call completed");
    }
   })
  }
}
