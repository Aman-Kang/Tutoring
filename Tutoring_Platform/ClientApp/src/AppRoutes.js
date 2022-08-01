import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { StudentAppointments } from "./components/StudentAppointments";
import { StudentLookForTutor } from "./components/StudentLookForTutor";
import { StudentHelp } from "./components/StudentHelp";
import { StudentAccount } from "./components/StudentAccount";

const AppRoutes = [
  
      {
        path: '/counter',
        element: <Counter />
      },
      {
        path: '/fetch-data',
        element: <FetchData />
      },
      {
        index: true,
        element: <StudentAppointments />
      },
    {
        path: '/look-for-tutor',
        element: <StudentLookForTutor />
    },
    {
        path: '/help',
        element: <StudentHelp />
    },
    {
        path: '/account',
        element: <StudentAccount />
    }

];

export default AppRoutes;
