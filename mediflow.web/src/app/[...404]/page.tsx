'use client';

import { RouteNames } from '@/lib/RouteNames';
import { redirect } from 'next/navigation';

function Index() {
  redirect(RouteNames.Index);
}

export default Index;
