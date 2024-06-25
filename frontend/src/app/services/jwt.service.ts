import { Injectable } from '@angular/core';
import { jwtDecode } from "jwt-decode";

interface JwtPayload {
  sub: string;
  nameid: string;
  unique_name: string;
  email: string;
  role: string | string[];
  exp: number;
}

@Injectable({
  providedIn: 'root'
})

export class JwtService {
  decodeToken(token: string): JwtPayload {
    const decoded: any = jwtDecode(token);
    return {
      sub: decoded.sub,
      nameid: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      unique_name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      exp: decoded.exp,
    };
  }

  getTokenExpirationDate(token: string): Date | null {
    const decoded = this.decodeToken(token);
    if (decoded.exp === undefined) return null;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  isTokenExpired(token: string): boolean {
    const date = this.getTokenExpirationDate(token);
    if (date === null) return false;
    return !(date.valueOf() > new Date().valueOf());
  }
}