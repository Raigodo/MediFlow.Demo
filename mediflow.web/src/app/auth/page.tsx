'use client';

import { RouteNames } from '@/lib/RouteNames';
import { redirect } from 'next/navigation';

function Index() {
  redirect(RouteNames.Login);
}

export default Index;
