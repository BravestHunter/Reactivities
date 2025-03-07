import { Grid, Loader } from 'semantic-ui-react'
import ActivityList from './ActivityList'
import { useMainStore } from '../../../app/stores/mainStore'
import { observer } from 'mobx-react-lite'
import { useEffect, useState } from 'react'
import ActivityFilters from './ActivityFilters'
import InfiniteScroll from 'react-infinite-scroller'
import ActivityListItemPlaceholder from './ActivityListItemPlaceholder'

export default observer(function ActivityDashboard() {
  const { activityStore } = useMainStore()
  const { loadNextActivitiesPage, hasMore, loading, isDirty } = activityStore
  const [loadingNext, setLoadingNext] = useState(false)

  function handleGetNext() {
    setLoadingNext(true)
    loadNextActivitiesPage().then(() => setLoadingNext(false))
  }

  useEffect(() => {
    if (isDirty) loadNextActivitiesPage()
  }, [isDirty, loadNextActivitiesPage])

  return (
    <Grid>
      <Grid.Column width="10">
        {loading && !loadingNext ? (
          <>
            <ActivityListItemPlaceholder />
            <ActivityListItemPlaceholder />
          </>
        ) : (
          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={!loadingNext && hasMore}
            initialLoad={false}
          >
            <ActivityList />
          </InfiniteScroll>
        )}
      </Grid.Column>
      <Grid.Column width="6">
        <ActivityFilters />
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  )
})
