import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private getLoginStatus: boolean;

  constructor(private account: AccountService) { }

  ngOnInit() {

    //this.getLoginStatus = this.account.

  }

}
