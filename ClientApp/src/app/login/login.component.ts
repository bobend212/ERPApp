import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  insertForm: FormGroup;
  userName: FormControl;
  password: FormControl;
  returnUrl: string;
  errorMessage: string;
  invalidLogin: boolean;

  constructor(
    private account: AccountService,
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder) { }

  ngOnInit() {

    this.userName = new FormControl('', [Validators.required]);
    this.password = new FormControl('', [Validators.required, Validators.minLength(4)]);

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    this.insertForm = this.formBuilder.group({
      "Username": this.userName,
      "Password": this.password
    });
  }

  onSubmit() {

    let userLogin = this.insertForm.value;

    this.account.login(userLogin.Username, userLogin.Password).subscribe(
      result =>
      {
      let token = (<any>result).token;
      console.log(token);  //show token in console <--
      console.log(result.userRole);
      console.log("Usser logged in successfully");
      this.invalidLogin = false;
      console.log(this.returnUrl);
      this.router.navigateByUrl(this.returnUrl);
      },
      error =>
      {
        this.invalidLogin = true;
        this.errorMessage = "Invalid data supplied";

        console.log(this.errorMessage);
      });

  }

}
