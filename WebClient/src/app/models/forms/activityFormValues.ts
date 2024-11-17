export default class ActivityFormValues {
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
