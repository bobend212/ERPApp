import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, from } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient, private router: Router) { }

  private baseUrlLogin: string = "/api/account/login";

  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());
  private userName = new BehaviorSubject<string>(localStorage.getItem('userName'));
  private userRole = new BehaviorSubject<string>(localStorage.getItem('userRole'));

  checkLoginStatus(): boolean
  {
    return false;
  }

  login(userName: string, password: string) {

    return this.httpClient.post<any>(this.baseUrlLogin, { userName, password }).pipe(

      map(result => {
        if (result && result.token)
        {
          this.loginStatus.next(true);
          localStorage.setItem('loginStatus', '1');
          localStorage.setItem('jwt', result.token);
          localStorage.setItem('username', result.username);
          localStorage.setItem('expiration', result.expiration);
          localStorage.setItem('userRole', result.userRole);
        }
        return result;
      }));
  }

  logout()
  {
    this.loginStatus.next(false);
    localStorage.removeItem('jwt');
    localStorage.removeItem('username');
    localStorage.removeItem('expiration');
    localStorage.removeItem('userRole');
    localStorage.setItem('loginStatus', '0');
    this.router.navigate(['/login']);
    console.log("user logged out successfully"); // show inside console information
  }

  get isLoggedIn()
  {
    return this.loginStatus.asObservable();
  }

  get currentUserName() {
    return this.userName.asObservable();
  }

  get currentUserRole() {
    return this.userRole.asObservable();
  }

}
