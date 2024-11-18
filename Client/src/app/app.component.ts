import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {


  title = 'Client';

  httpClient = inject(HttpClient);

user:any;

  ngOnInit(): void {
     this.httpClient.get('https://localhost:5001/api/Users/GetUsers').subscribe({
      next: (response) => { 
        this.user=response;
      },
      error: (err) => {console.log(err) },
      complete: () => { console.log('request has been completed')}
    });


  }
}
