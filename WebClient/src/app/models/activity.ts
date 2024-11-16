import Attendee from './attendee'

export interface Activity {
  id: number
  title: string
  date: Date
  description?: string
  category: string
  city: string
  venue: string
  isCancelled: boolean
  host: Attendee
  attendees: Attendee[]

  isHost: boolean
  isGoing: boolean
}

export class Activity implements Activity {
  constructor(init?: ActivityFormValues) {
    Object.assign(this, init)
  }
}

export class ActivityFormValues {
  id?: number = undefined
  title: string = ''
  date: Date | null = null
  description?: string = undefined
  category: string = ''
  city: string = ''
  venue: string = ''

  constructor(activity?: ActivityFormValues) {
    if (activity) {
      this.id = activity.id
      this.title = activity.title
      this.date = activity.date
      this.description = activity.description
      this.category = activity.category
      this.city = activity.city
      this.venue = activity.venue
    }
  }
}
