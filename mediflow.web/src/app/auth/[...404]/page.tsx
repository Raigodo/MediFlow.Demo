'use client';

import { RouteNames } from '@/lib/RouteNames';
import { redirect } from 'next/navigation';

function NotFound() {
  redirect(RouteNames.Login);
}

export default NotFound;
